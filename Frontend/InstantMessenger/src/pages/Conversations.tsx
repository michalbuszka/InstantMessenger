import { useEffect } from "react"
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
            <div className="conversationsList">
                <input type="text" placeholder="Search conversations..." />
            </div>
            <div className="conversation">
                <div className="messages"></div>
                <div className="messageType">
                    <input type="text" placeholder="Type a message..." />
                    <button>Send</button>
                </div>
            </div>
            <div className="friendDetails"></div>
                
        </div>
    )
}

export default Conversations