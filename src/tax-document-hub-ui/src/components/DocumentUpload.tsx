import React, {type ChangeEvent, useState} from 'react';

interface Category{
    id: string,
    name: string,
}
interface DocumentUploadProps {
    categories: Category[];
    onUploadSuccess: () => void;
}

export default function DocumentUpload({categories, onUploadSuccess}: DocumentUploadProps) {
    console.log("Categories ", categories.length);
    const [title, setTitle] = useState('');
    const [selectedCategoryId, setSelectedCategoryId] = useState('');
    const [file, setFile] = useState<File | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [message, setMessage] = useState<string | null>(null);
    
    const handleFileChange = (event: ChangeEvent<HTMLInputElement>) => {
        if(event.target.files && event.target.files[0]) {
            setFile(event.target.files[0]);
        }
    }
    
    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        setError(null);
        setMessage(null);
        if(!title || !selectedCategoryId || file === null){
            setError('Bitte fühlen Sie alle Felder aus und wähle eine PDF-Datei')
            return;
        }
        
        const formData = new FormData();
        formData.append('title', title);
        formData.append('file', file);
        formData.append('categoryId', selectedCategoryId);
        
        const token = localStorage.getItem('token');
        try{
            const response = await fetch('http://localhost:5032/api/documents/upload', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
                body: formData,
            });
            if(!response.ok){
                const errorText = await response.text();
                throw new Error(errorText || 'Upload fehlgeschlagen');
            }
            
            setMessage('Document erfolgreich hochgeladen');
            setTitle('');
            setFile(null);
            onUploadSuccess();
        }catch(error: any){
            setError(error.message);
        }
    }
    
    return (
        <div className="card">
            <h2 className="card-title">Neues Steuerdokument hochladen</h2>
            {error && <div className="alert alert-error">{error}</div>}
            {message && <div className="alert alert-success">{message}</div>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label className="form-label" htmlFor="title">Titel</label>
                    <input className="form-input" type="text"
                           id="title"
                           value={title}
                           onChange={(e)=> setTitle(e.target.value)}
                           placeholder="z. B. Lohnsteuer 2025"/>
                </div>
                <div className="form-group">
                    <label className="form-label" htmlFor="categorySelect">Kategorie</label>
                    <select
                        className="form-select"
                        id="categorySelect"
                        value={selectedCategoryId}
                        onChange={(e)=> setSelectedCategoryId(e.target.value)}
                        >
                        <option value="">Bitte wählen</option>
                        {categories.map(category => (
                            <option key={category.id} value={category.id}>{category.name}</option>
                        ))}
                    </select>
                </div>
                <div className="form-group">
                    <label className="form-label" htmlFor="file">PDF-Datei</label>
                    <input className="form-file" type="file"
                    id="file"
                    accept="application/pdf"
                    onChange={handleFileChange}
                    />
                </div>
                <button className="btn btn-primary" type="submit">Hochladen</button>
            </form>
        </div>
    )
}