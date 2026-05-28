import '../Styles/Login.css'
import '../Styles/Global.css'
import axios from 'axios'; // 1. Dodany import Axiosa
import { useEffect, useRef, useState } from 'react'

function Register () {
    const [messages, setMessages] = useState<string[]>([]);
    const nickRef = useRef<HTMLInputElement>(null);
    const usernameRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    const saveToken = (token: string, refreshToken: string) => {
        localStorage.setItem('token', token);
        localStorage.setItem('refreshToken', refreshToken);
    }

    const handleRegister = async () => {
        setMessages([]);
        try {
            const response = await axios.post('/api/LoginRegister/register', {
                nick: nickRef.current?.value,
                username: usernameRef.current?.value,
                password: passwordRef.current?.value
            });

            const data = response.data;

            if (Array.isArray(data.messages)) {
                setMessages(data.messages);
            } else if (data.message) {
                setMessages([data.message]);
            } else {
                setMessages(['Wystąpił błąd podczas rejestracji.']);
            }

            if (data.status === 0) {
                saveToken(data.token, data.refreshToken);
                window.location.href = '/conversations';
            }
        } catch (error: any) {
            console.error(error);
            
            if (error.response && error.response.data) {
                const serverData = error.response.data;
                if (Array.isArray(serverData.messages)) {
                    setMessages(serverData.messages);
                } else if (serverData.message) {
                    setMessages([serverData.message]);
                } else {
                    setMessages(['Serwer zwrócił błąd rejestracji.']);
                }
            } else {
                setMessages(['Nie można połączyć się z serwerem.']);
            }
        }
    }

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            window.location.href = '/';
        }
    }, []);

    return (
        <div className='container'>
            <div className='login'>
                <h1>Register</h1>
                <input type="text" placeholder='Username' ref={usernameRef} />
                <input type="text" placeholder='Nick' ref={nickRef} />
                <input type="password" placeholder='Password' ref={passwordRef} />
                <a href='/login'>Already have an account? Login</a>
                <div className='errorMessages'>
                    {messages.length > 0 && (
                        <ul>
                            {messages.map((message, index) => (
                                <li key={index}>{message}</li>
                            ))}
                        </ul>
                    )}
                </div>
                <button onClick={handleRegister}>Register</button>
            </div>
        </div>
    )
}

export default Register;