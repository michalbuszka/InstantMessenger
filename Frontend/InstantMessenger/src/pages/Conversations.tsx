import { useEffect } from "react"
import ConversationsList from "../components/ConversationList"
import Conversation from "../components/Conversation"
import FriendDetails from "../components/FriendDetails"
import '../Styles/Global.css'
import '../Styles/Conversations.css'

function Conversations() {
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) {
            window.location.href = '/login';
        }
        
    }, []);
    return (
        <div className="messengerContainer">
            <ConversationsList />
            <Conversation />
            <FriendDetails />
        </div>
    )
}

export default Conversations;