import '../Styles/Global.css'
import '../Styles/Conversation.css'
import Message from '../components/Message.tsx';
import { useParams } from 'react-router-dom';
import { useEffect, useRef, useState } from 'react';
import api, { getAccessToken, checkAuthWithBackend } from '../api/api.tsx';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

interface User {
    id: string,
    avatar: string,
    nick: string
}

function Conversation() {
    const [user, setUser] = useState<User | null>(null);
    const messageRef = useRef<HTMLInputElement>(null);
    const [connection, setConnection] = useState<HubConnection>(null);
    const getUser = async (id: string) => {
        const response = await api.get(`/api/User/getUser/${id}`);
        setUser(response.data);
    }
    const sendMessage = () => {
        const messageText = messageRef.current?.value;
        connection.send("SendMessage", "ID", messageText);
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
                    console.log('Połączono z SignalR!');
                    connection.on('ReceiveMessage', (userId, message) => {
                        console.log(message);
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
    return (
        <div className="conversation">
            <div className='conversationHeader'>
                <div className="profilePicture">
                    <img src={user?.avatar} alt="Awatar" className='contactAvatar'></img>
                </div>
                <h2>{user?.nick}</h2>
            </div>
            <div className="messages">
                <Message sender='Michał Buszka' content='siema' messageClass='myMessage' date='12:11' />
                <Message sender='Michał Buszka' content='siema' messageClass='theirMessage' date='12:11' />
                <Message date='12:11' sender='Michał Buszka' content='sidqwdqwd qwd qwdqwdqwdqwdqwd q dqwdqwdqwdqwdqwdqwdqwdw dqwema' messageClass='myMessage' />
                <Message date='12:11' sender='Michał Buszka' content='Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc laoreet odio in nibh semper, id efficitur leo tempus. Ut semper mi a tristique imperdiet. Vestibulum erat leo, mollis ac faucibus id, interdum a purus. In dignissim aliquet turpis, a blandit urna vestibulum sit amet. Suspendisse turpis tellus, mattis a mauris vel, congue dignissim mi. Ut vitae enim cursus, lacinia lacus quis, molestie massa. Nullam sed ligula sagittis, lobortis mi et, ullamcorper nulla. Suspendisse erat nunc, blandit in nisl sit amet, tempus posuere justo. Morbi ac maximus eros. Vivamus volutpat sem sed porttitor auctor. Vivamus vestibulum dui mollis vestibulum mollis.

Suspendisse tincidunt, libero et pretium dictum, elit risus fermentum metus, vestibulum ultrices urna nunc at ipsum. Aliquam convallis turpis at ullamcorper mollis. Morbi sit amet urna eget lorem gravida tristique nec in lectus. Nullam at porttitor dui. Aliquam congue vehicula tellus, venenatis tristique metus maximus quis. Nullam at interdum metus. Proin pretium gravida libero, eu euismod tellus posuere et. Fusce tincidunt feugiat ex.

Morbi lobortis justo volutpat posuere rutrum. Nam cursus turpis ac quam condimentum convallis sed nec nibh. Proin eget tincidunt nulla. Nullam luctus nec lectus non aliquam. Aliquam venenatis condimentum est sed volutpat. Mauris at justo ac ex faucibus egestas id in nisi. Phasellus sagittis sollicitudin orci, eu consectetur est congue maximus. Sed orci tellus, faucibus id sodales sed, feugiat id urna.

Vivamus purus nisi, aliquet sit amet est nec, ullamcorper posuere enim. Integer sodales neque consectetur euismod ullamcorper. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Sed laoreet ultrices orci, vitae suscipit magna tempor in. Quisque massa augue, dictum nec sodales et, dignissim id orci. Etiam fringilla urna blandit eros ultricies, ac mattis neque dapibus. Suspendisse a vestibulum justo, a venenatis diam. Pellentesque ut massa eget mi posuere rhoncus. Integer nec mauris rhoncus leo accumsan mollis. Vivamus tempus, velit ac tempor imperdiet, dui nibh bibendum ex, sed tincidunt enim diam in sem. Proin quis augue mollis, ornare risus in, sagittis nisl. Praesent convallis sem id libero venenatis commodo. Fusce ac rhoncus metus. Nam varius congue leo finibus porttitor.

Sed accumsan massa at eleifend bibendum. Suspendisse eget tempor purus. Nam justo massa, facilisis ac massa at, sodales feugiat quam. Suspendisse id odio leo. Cras vel tortor laoreet, hendrerit dui nec, lacinia ex. Mauris fermentum scelerisque dui at vestibulum. Nulla bibendum sollicitudin urna, in pellentesque sapien ultrices vel. Nam vehicula leo ut nibh malesuada finibus. Nunc a porttitor arcu. Etiam hendrerit purus dictum, blandit massa eget, fermentum sem.' messageClass='theirMessage' />
            </div>
            <div className="messageType">
                <input ref={messageRef} type="text" placeholder="Type a message..." />
                <button onClick={sendMessage}>Send</button>
            </div>
        </div>
    )
}

export default Conversation;