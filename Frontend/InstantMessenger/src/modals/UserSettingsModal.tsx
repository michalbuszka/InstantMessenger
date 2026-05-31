import { useState, useEffect } from 'react';
import type { ChangeEvent } from 'react';
import ReactDOM from 'react-dom';
import '../Styles/UserSettingsModal.css';
import api from '../api/api';

export interface UserData {
    email: string;
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
    if (!isOpen) return null;

    const [formData, setFormData] = useState<UserData>(initialData || {
        email: '',
        firstName: '',
        lastName: '',
        nick: '',
        avatar: ''
    });

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, files } = e.target;
        if (type === 'file' && files && files[0]) {
            const file = files[0];
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result as string;
                setFormData(prev => ({ ...prev, [name]: base64String }));
            };
            reader.readAsDataURL(file);
        } else {
            setFormData(prev => ({ ...prev, [name]: value }));
        }
    };

    const handleSave = () => {
        onSave(formData);
    };

    const getData = async () => {
        const response = await api.get('api/User/getUserData');
        const data = response.data;
        setFormData(prev => ({
            ...prev,
            email: data.email || '',
            firstName: data.firstName || '',
            lastName: data.lastName || '',
            nick: data.nick || ''
        }));
    }

    useEffect(() => {
        getData();
    }, []);

    return ReactDOM.createPortal(
        <div className="modal-overlay" onClick={onClose}>
            <div className="userSettingsModal" onClick={(e) => e.stopPropagation()}>
                <h3>User settings</h3>
                
                <label htmlFor="firstName">Imię</label>
                <input 
                    type="text" 
                    name="firstName" 
                    placeholder="Imię" 
                    value={formData.firstName} 
                    onChange={handleChange} 
                />
                <label htmlFor="lastName">Nazwisko</label>
                <input 
                    type="text" 
                    name="lastName" 
                    placeholder="Nazwisko" 
                    value={formData.lastName} 
                    onChange={handleChange} 
                />
                <label htmlFor="nick">Nick</label>
                <input 
                    type="text" 
                    name="nick" 
                    placeholder="Nick (pseudonim)" 
                    value={formData.nick} 
                    onChange={handleChange} 
                />
                <label htmlFor="email">Email</label>
                <input 
                    type="email" 
                    name="email" 
                    placeholder="Adres e-mail" 
                    value={formData.email} 
                    onChange={handleChange} 
                />
                <label htmlFor='avatar'>Avatar img:</label>
                <input 
                    type="file" 
                    name="avatar" 
                    placeholder="Avatar" 
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