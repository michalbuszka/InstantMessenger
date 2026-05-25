import { useEffect } from "react"
import ConversationsList from "../components/ConversationList"
import Conversation from "../components/Conversation"
import FriendDetails from "../components/FriendDetails"
import '../Styles/Global.css'
import '../Styles/Conversations.css'
import { useState } from "react"
import UserSettingsModal from "../modals/UserSettingsModal"

function Conversations() {
    const [isUserSettingsModalOpen, setIsUserSettingsModalOpen] = useState(false);
    
    const handleSaveSettings = async (data: any) => {
        const token = localStorage.getItem('token');
        if (token == null) {
            alert("Błąd!");
            return;
        }
        setIsUserSettingsModalOpen(false); 
        var response = await fetch('/api/User/updateUserData', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(data)
        });
        if (response.status == 200)
            alert("Dane zostały zapisane!");
    };

    const logout = () => {
        localStorage.removeItem('token');
        window.location.href = '/login';
    }

    return (
        <div className="messengerContainer">
            <UserSettingsModal 
                isOpen={isUserSettingsModalOpen} 
                onClose={() => setIsUserSettingsModalOpen(false)} 
                onSave={handleSaveSettings}
            />

            <div className="leftPanel">
                <ConversationsList />
                <div className="userSettings">
                    <button onClick={() => setIsUserSettingsModalOpen(true)}>Settings</button>
                    <button onClick={logout}>Logout</button>
                </div>
            </div>
            <Conversation />
            <FriendDetails />
        </div>
    )
}

export default Conversations;