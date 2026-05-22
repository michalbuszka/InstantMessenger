import { useEffect } from "react"
import ConversationsList from "../components/ConversationList"
import Conversation from "../components/Conversation"
import FriendDetails from "../components/FriendDetails"
import '../Styles/Global.css'
import '../Styles/Conversations.css'
import { useState } from "react"

function Conversations() {
    const [isUserSettingsModalOpen, setIsUserSettingsModalOpen] = useState(false);
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
            <div className="leftPanel">
                <ConversationsList />
                <div className="userSettings">
                    <button onClick={() => {setIsUserSettingsModalOpen(true)}}>Settings</button>
                    <button onClick={() => {logout()}}>Logout</button>
                </div>
            </div>
            <Conversation />
            <FriendDetails />
        </div>
    )
}

export default Conversations;