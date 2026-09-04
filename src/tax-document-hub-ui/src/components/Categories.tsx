import React from 'react';
import {useState} from 'react';
import type {Category} from "../models/category.ts";

interface CategoryProps {
    categories: Category[];
    onAddCategory: (newCategory: Category) => void;
}


export default function Categories({categories, onAddCategory}: CategoryProps ) {

    const [newCategoryName, setNewCategoryName] = useState('');
    const [error, setError] = useState('');


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        if(!newCategoryName.trim()) {
            setError('Kategorie name ist erforderlich');
            return;
        }
        var newCategory = {
            name: newCategoryName,
            id:''    
        }
        onAddCategory(newCategory);
        setNewCategoryName('')
        
    }
    
    return (
        <div className="card">
            <h2 className="card-title">Kategorien</h2>
            {error && <div className="alert alert-error">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-row">
                    <div className="form-group">
                        <label className="form-label" htmlFor='categoryName'>Neue Kategorie</label>
                        <input className="form-input" type="text"
                        value={newCategoryName}
                        onChange={(e)=> setNewCategoryName(e.target.value)}
                        id="categoryName"
                        placeholder="z. B. Einkommensteuer"/>
                    </div>
                    <button className="btn btn-primary" type="submit">Hinzufügen</button>
                </div>
            </form>

            <div className="section-divider"/>

            {categories.length === 0 ? (
                <div className="empty-state">Noch keine Kategorien vorhanden</div>
            ) : (
                <ul className="category-list">
                    {categories.map((category)=> (
                        <li key={category.id} className="category-item">{category.name}</li>
                    ))}
                </ul>
            )}
        </div>
    )
    
}