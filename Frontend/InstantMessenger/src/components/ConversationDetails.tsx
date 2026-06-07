import '../Styles/Global.css'
import '../Styles/ConversationDetails.css'
import api from '../api/api.tsx';
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify';

interface ConversationDetailsProps {
    conversationId: string
}

interface ConversatrionUser {
    id: string,
    avatar: string,
    nick: string
}

function FriendDetails(props : ConversationDetailsProps) {
    const getConversationUsers = async () => {
        const response = await api.get(`/api/Conversation/getConversationUsers/${props.conversationId}`);
        console.log(response);
        setConversationUsers(response.data);
    }
    const changeNick = async (id: string, newNick: string) => {
        const response = await api.patch(`/api/Conversation/editNick`, {
            id: id,
            newNick: newNick   
        });
        if (response.status == 200)
        {
            await getConversationUsers();
           toast.success("The data has been saved!");
        }
    }
    const chandleNickChange = (id : string, newNick : string) => {
        setConversationUsers(prevUsers => 
            prevUsers.map( user => user.id === id ? {...user, nick: newNick} : user)
        );
    }
    useEffect(()=>{getConversationUsers()}, [props.conversationId])
    const [convesationUsers, setConversationUsers] = useState<ConversatrionUser[]>([]);
    return (
        <div className="friendDetails">
            <h2>Conversation settings</h2>
            <div className='friendDetailsNicks'>
                
            </div>
            {
                convesationUsers.map((user, key) => {
                    return (
                        <div className='friendDetailsNicks' key={key}>
                            <div className="profilePicture">
                                <img src={user.avatar} className='contactAvatar'></img>
                            </div>
                            <input type='text' value={user.nick} onChange={(e) => {chandleNickChange(user.id, e.target.value)}}/>
                            <button onClick={ () => { changeNick(user.id, user.nick) }}>Save</button>
                        </div>
                    );
                })
            }
        </div>   
    );
}

export default FriendDetails;