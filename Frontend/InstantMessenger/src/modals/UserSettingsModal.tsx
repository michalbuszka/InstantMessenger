import React, { useState, ChangeEvent } from 'react';
import ReactDOM from 'react-dom';
import '../Styles/UserSettingsModal.css';

export interface UserData {
    email: string;
    password?: string; // Hasło jest opcjonalne przy edycji
    firstName: string;
    lastName: string;
    nick: string;
    avatar: string;
}

interface UserSettingsModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSave: (data: UserData) => void;
    initialData?: UserData;
}

function UserSettingsModal({ isOpen, onClose, onSave, initialData }: UserSettingsModalProps) {
    // Inicjalizacja stanu danymi początkowymi lub pustymi wartościami
    const [formData, setFormData] = useState<UserData>(initialData || {
        email: '',
        firstName: '',
        lastName: '',
        nick: '',
        avatar: ''
    });

    if (!isOpen) return null;

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSave = () => {
        onSave(formData);
    };

    return ReactDOM.createPortal(
        <div className="modal-overlay" onClick={onClose}>
            <div className="userSettingsModal" onClick={(e) => e.stopPropagation()}>
                <h3>Ustawienia użytkownika</h3>
                
                <div className="form-group">
                    <input 
                        type="text" 
                        name="firstName" 
                        placeholder="Imię" 
                        value={formData.firstName} 
                        onChange={handleChange} 
                    />
                    <input 
                        type="text" 
                        name="lastName" 
                        placeholder="Nazwisko" 
                        value={formData.lastName} 
                        onChange={handleChange} 
                    />
                </div>

                <input 
                    type="text" 
                    name="nick" 
                    placeholder="Nick (pseudonim)" 
                    value={formData.nick} 
                    onChange={handleChange} 
                />

                <input 
                    type="email" 
                    name="email" 
                    placeholder="Adres e-mail" 
                    value={formData.email} 
                    onChange={handleChange} 
                />

                <input 
                    type="text" 
                    name="avatar" 
                    placeholder="URL Avatara" 
                    value={formData.avatar} 
                    onChange={handleChange} 
                />

                <div className="modal-actions">
                    <button onClick={onClose} className="btn-close">Zamknij</button>
                    <button onClick={handleSave} className="btn-save">Zapisz</button>
                </div>
            </div>
        </div>,
        document.body
    );
}

export default UserSettingsModal;