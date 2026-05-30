import ConversationsList from "../components/ConversationList"
import Conversation from "../components/Conversation"
import FriendDetails from "../components/FriendDetails"
import '../Styles/Global.css'
import '../Styles/Conversations.css'
import { useState } from "react"
import api from '../api/api.tsx';
import UserSettingsModal from "../modals/UserSettingsModal"

function Conversations() {
    const [isUserSettingsModalOpen, setIsUserSettingsModalOpen] = useState(false);
    
    const handleSaveSettings = async (data: any) => {
        setIsUserSettingsModalOpen(false); 
        var response = await api.post('/api/User/updateUserData', data);
        if (response.status == 200)
            alert("The data has been saved!");
    };

    const logout = () => {
        // localStorage.removeItem('token');
        // window.location.href = '/login';
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