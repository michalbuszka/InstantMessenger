import '../Styles/Global.css'
import '../Styles/ConversationList.css'
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
        const response = await fetch(`/api/User/getUserContacts/${query}`);
        const data = await response.json();
        setContacts(data); //celowo bez spread operatora, żeby lista kontaktów się odświeżała ;) 
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