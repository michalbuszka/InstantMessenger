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
    
    const handleSaveSettings = (data: any) => {
        console.log("Nowe dane użytkownika:", data);
        setIsUserSettingsModalOpen(false); // Zamykamy po zapisie
    };

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) {
            window.location.href = '/login';
        }
    }, []);

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