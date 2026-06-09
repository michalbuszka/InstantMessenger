import '../Styles/Global.css'
import '../Styles/Conversation.css'
import MessageComponent from '../components/Message.tsx'; 
import { useParams, useNavigate } from 'react-router-dom';
import React, { useEffect, useRef, useState } from 'react';
import api, { getAccessToken, getUserId } from '../api/api.tsx';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

interface User {
    id: string,
    avatar: string,
    nick: string
}

interface Message {
    senderId: string,
    nick: string,
    content: string,
    date: string,
    conversationId: string
}

function Conversation() {
    const initUser : User = {
        id: '',
        avatar: '',
        nick: 'Login'
    }
    const navigate = useNavigate();
    const [user, setUser] = useState<User | null>(initUser);
    const [messagesList, setMessagesList] = useState<Message[]>([])
    const messageRef = useRef<HTMLInputElement>(null);
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const { id } = useParams<{ id: string }>();

    const getUser = async (id: string) => {
        const response = await api.get(`/api/User/getUser/${id}`);
        setUser(response.data);
    }
    
    const getMessages = async (currentId: string) => {
        const result = await api.get(`/api/Conversation/messages/${currentId}`);
        const sorted = result.data.sort((a: Message, b: Message) => new Date(a.date).getTime() - new Date(b.date).getTime());
        setMessagesList(sorted);
    }

    const sendMessage = () => {
        const messageContent = messageRef.current?.value;
        if (connection && messageContent?.trim()) {
            if (messageRef.current) {
                connection.send("SendMessage", id, messageContent);
                messageRef.current.value = "";
            }
        }
    }

    useEffect(() => {
        let isMounted = true;
        
        const startConnection = async () => {
            const token = await getAccessToken();
            const newConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5199/conversationHub', {
                    accessTokenFactory: () => token
                })
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            try {
                await newConnection.start();
                if (isMounted) {
                    setConnection(newConnection);
                }
            } catch (err) {
                console.log('Błąd połączenia SignalR: ', err);
            }
        };

        startConnection();

        return () => {
            isMounted = false;
            if (connection) {
                connection.stop();
            }
        };
    }, []);

    useEffect(() => {
        if (id) {
            setMessagesList([]);
            getUser(id);
            getMessages(id);
        }
    }, [id]);

    useEffect(() => {
        if (!connection) return;

        connection.on('ReceiveMessage', (userId, nick, messageContent, date, conversationId) => {
            const newMessage : Message = {
                senderId: userId,
                nick: nick,
                content: messageContent,
                date: date,
                conversationId: conversationId
            }
            setMessagesList(prevMessages => [...prevMessages, newMessage]);
            if (id != conversationId)
            {
                //navigate(`/${conversationId}`);
            }
        });

        return () => {
            connection.off('ReceiveMessage');
        };
    }, [connection]);

    const messagesContainerRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        const el = messagesContainerRef.current;
        if (!el) return;
        el.scrollTop = el.scrollHeight;
    }, [messagesList]);

    const getMessageClass = (senderId : string) => {
        return senderId === getUserId() ? 'myMessage' : 'otherMessage';
    }

    return (
        <div className="conversation">
            <div className='conversationHeader'>
                <div className="profilePicture">
                    <img src={user?.avatar} alt="Awatar" className='contactAvatar'></img>
                </div>
                <h2>{user?.nick}</h2>
            </div>
            <div className="messages" ref={messagesContainerRef}>
                {
                    messagesList.map((message, key) => (
                        <MessageComponent 
                            key={key} 
                            sender={message.nick} 
                            content={message.content} 
                            messageClass={getMessageClass(message.senderId)} 
                            date={message.date}
                        />
                    ))
                }
            </div>
            <div className="messageType">
                <input ref={messageRef} type="text" placeholder="Type a message..." onKeyDown={e => {
                    if (e.key === 'Enter') sendMessage();
                }} />
                <button onClick={sendMessage}>Send</button>
            </div>
        </div>
    )
}

export default Conversation;