import '../Styles/Global.css'
import '../Styles/ConversationList.css'
import axios from 'axios';
import { useState, type ChangeEvent } from 'react';

function ConversationsList() {
    interface User 
    {
        id : string,
        avatar: string,
        nick: string
    }
    const [contacts, setContacts] = useState<User[]>([]);
    const serachConversations = async (query : string) => {
        const response = await axios.get(`/api/User/getUserContacts/${query}`);
        setContacts(response.data); 
    }

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        serachConversations(e.target.value);
    }
    return (
        <div className="conversationList">
            <input type="text" placeholder="Search conversations..." className="searchConversations" onChange={handleChange}/>
            <div className="profileList">
                {
                    contacts.map((contact : User, key) => {
                        return (
                        <div className="profile" key={key}>
                            <div className="profilePicture">
                                <img src={contact.avatar} alt="Awatar" className='contactAvatar'></img>
                            </div>
                            <b>{contact.nick}</b>
                        </div>);
                    })
                }
            </div>
        </div>
    );
}

export default ConversationsList;