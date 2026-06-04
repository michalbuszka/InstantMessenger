import '../Styles/Global.css'
import '../Styles/Conversation.css'
import Message from '../components/Message.tsx';
import { useParams } from 'react-router-dom';
import { useEffect, useRef, useState } from 'react';
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
    date: string
}

function Conversation() {
    const initUser : User = {
        id: '',
        avatar: '',
        nick: 'Login'
    }
    const [user, setUser] = useState<User | null>(initUser);
    const [messagesList, setMessagesList] = useState<Message[]>([])
    const messageRef = useRef<HTMLInputElement>(null);
    const [connection, setConnection] = useState<HubConnection>();
    const getUser = async (id: string) => {
        const response = await api.get(`/api/User/getUser/${id}`);
        setUser(response.data);
    }
    const sendMessage = () => {
        const messageContent = messageRef.current?.value;
        if (connection)
            connection.send("SendMessage", id, messageContent);
    }
    const connectToSignalR = (accessToken : string) => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5199/conversationHub', {
                accessTokenFactory: () => accessToken
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        setConnection(newConnection);
    }
    const connect = async () => {
        connectToSignalR(await getAccessToken());
    }
    useEffect(() => {
        connect();
    }, []);
    const { id } = useParams<{ id: string }>();
    useEffect(() => {
        if (id != null)
            getUser(id);
    }, [id])
    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    connection.on('ReceiveMessage', (userId, nick, messageContent, date) => {
                        const newMessage : Message = {
                            senderId: userId,
                            nick: nick,
                            content: messageContent,
                            date: date
                        }
                        setMessagesList(messagesList => [...messagesList, newMessage])
                    });
                })
                .catch(e => console.log('Błąd połączenia: ', e));
        }

        return () => {
            if (connection) {
                connection.off('ReceiveMessage');
                connection.stop();
            }
        };
    }, [connection]);
    const getMessageClass = (senderId : string) => {
        console.log(getUserId() + " " + senderId);
        if (senderId === getUserId())
            return 'myMessage';
        return 'otherMessage';
    }
    return (
        <div className="conversation">
            <div className='conversationHeader'>
                <div className="profilePicture">
                    <img src={user?.avatar} alt="Awatar" className='contactAvatar'></img>
                </div>
                <h2>{user?.nick}</h2>
            </div>
            <div className="messages">
                {
                    messagesList.map((message, key) => {
                        return(
                        <>
                            <Message sender={message.nick} content={message.content} messageClass={getMessageClass(message.senderId)} date={message.date}/>
                        </>)
                    })
                }
            </div>
            <div className="messageType">
                <input ref={messageRef} type="text" placeholder="Type a message..." />
                <button onClick={sendMessage}>Send</button>
            </div>
        </div>
    )
}

export default Conversation;