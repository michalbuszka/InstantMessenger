import '../Styles/Login.css'
import '../Styles/Global.css'
import axios from 'axios';
import { useEffect, useRef, useState } from 'react'

function Login () {
    const [messages, setMessages] = useState<string[]>([]);
    const usernameRef = useRef<HTMLInputElement>(null);
    const passwordRef = useRef<HTMLInputElement>(null);

    const saveToken = (token: string) => {
        localStorage.setItem('token', token);
    }

    const handleLogin = async () => {
        setMessages([]);
        try {
            const response = await axios.post('/api/LoginRegister/login', {
                username: usernameRef.current?.value,
                password: passwordRef.current?.value
            });
            const data = response.data;
            if (Array.isArray(data.messages)) {
                setMessages(data.messages);
            } else if (data.message) {
                setMessages([data.message]);
            } else {
                setMessages(['Wystąpił błąd podczas logowania.']);
            }

            if (data.status === 0) {
                saveToken(data.token);
                window.location.href = '/';
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
                    setMessages(['Serwer zwrócił błąd logowania.']);
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
                <h1>Login</h1>
                <input type="text" placeholder='Username' ref={usernameRef} />
                <input type="password" placeholder='Password' ref={passwordRef} />
                <a href='/register'>Sign up</a>
                <div className='errorMessages'>
                    {messages.length > 0 && (
                        <ul>
                            {messages.map((message, index) => (
                                <li key={index}>{message}</li>
                            ))}
                        </ul>
                    )}
                </div>
                <button onClick={handleLogin}>Login</button> 
            </div>
        </div>
    )
}

export default Login