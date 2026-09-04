import {useState, useEffect} from 'react'

import { useNavigate } from 'react-router-dom'
import Categories from './components/Categories';

import type {Category} from "./models/category.ts";
import DocumentUpload from "./components/DocumentUpload.tsx";

type Section = 'overview' | 'categories' | 'upload';

const SECTIONS: { id: Section; label: string; icon: string }[] = [
    { id: 'overview',   label: 'Übersicht',  icon: '🏠' },
    { id: 'categories', label: 'Kategorien', icon: '🗂️' },
    { id: 'upload',     label: 'Hochladen',  icon: '⬆️' },
];

export default function Dashboard(){

    const navigate = useNavigate();
    const [categories, setCategories] = useState<Category[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [activeSection, setActiveSection] = useState<Section>('overview');
    const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

    const fetchCategories = async () => {
        setError(null);
        try{
            const response = await fetch('http://localhost:5032/api/category', {
                method: 'GET',
                headers:{
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(errorText || 'Kategorien konnten nicht geladen werden');
            }

            const data = await response.json();
            setCategories(data);
        }catch(err: any){
            setError(err.message);
        }
    };

    const addCategory = async (category: Category): Promise<void> => {
        setError(null);
        try{
            const response = await fetch('http://localhost:5032/api/category', {
                method: 'POST',
                headers:{
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify({ name: category.name })
            });
            if(!response.ok){
                const errorText = await response.text();
                throw new Error(errorText || 'Kategorie konnte nicht angelegt werden');
            }
            await fetchCategories();
        }catch(err: any){
            setError(err.message);
        }
    }

    const onUploadSuccess = async () => {
        setActiveSection('overview');
    }

    useEffect(() => {
        fetchCategories();
    }, []);

    const handleLogout = () =>{
        localStorage.removeItem('token')
        navigate('/login')
    }

    return(
        <div className="app-shell">
            <header className="topbar">
                <div className="topbar-brand">
                    <span className="topbar-logo">📄</span>
                    <span className="topbar-title">Steuerdokumente</span>
                </div>
                <div className="topbar-user">
                    <span className="topbar-greeting">Hallo!</span>
                    <button className="btn btn-secondary" type='button' onClick={handleLogout}>
                        Abmelden
                    </button>
                </div>
            </header>

            <div className={`layout${sidebarCollapsed ? ' is-collapsed' : ''}`}>
                <aside className="sidebar">
                    <button
                        type="button"
                        className="sidebar-toggle"
                        onClick={() => setSidebarCollapsed(c => !c)}
                        aria-label={sidebarCollapsed ? 'Sidebar ausklappen' : 'Sidebar einklappen'}
                        title={sidebarCollapsed ? 'Ausklappen' : 'Einklappen'}
                    >
                        {sidebarCollapsed ? '»' : '«'}
                    </button>
                    <nav className="sidebar-nav">
                        {SECTIONS.map(section => (
                            <button
                                key={section.id}
                                type="button"
                                className={`sidebar-item${activeSection === section.id ? ' is-active' : ''}`}
                                onClick={() => setActiveSection(section.id)}
                                title={sidebarCollapsed ? section.label : undefined}
                            >
                                <span className="sidebar-icon" aria-hidden="true">{section.icon}</span>
                                <span className="sidebar-label">{section.label}</span>
                            </button>
                        ))}
                    </nav>
                </aside>

                <main className="content">
                    {error && <div className="alert alert-error">{error}</div>}

                    {activeSection === 'overview' && (
                        <div className="card">
                            <h2 className="card-title">Übersicht</h2>
                            <p style={{ color: 'var(--text-muted)', margin: 0 }}>
                                Willkommen in deinem geschützten Steuerdokumenten-Bereich.
                            </p>
                            <div className="stats">
                                <div className="stat-tile">
                                    <span className="stat-value">{categories.length}</span>
                                    <span className="stat-label">Kategorien</span>
                                </div>
                            </div>
                        </div>
                    )}

                    {activeSection === 'categories' && (
                        <Categories categories={categories} onAddCategory={addCategory}/>
                    )}

                    {activeSection === 'upload' && (
                        <DocumentUpload
                            categories={categories}
                            onUploadSuccess={onUploadSuccess}
                        />
                    )}
                </main>
            </div>
        </div>
    )
}
