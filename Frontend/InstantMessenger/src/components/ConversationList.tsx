import '../Styles/Global.css'
import '../Styles/ConversationList.css'

function ConversationsList() {
    const logout = () => {
        localStorage.removeItem('token');
        window.location.href = '/login';
    }
    return (
        <div className="conversationList">
            <input type="text" placeholder="Search conversations..." className="searchConversations" />
            <div className="profileList">
                <div className="profile">
                    <div className="profilePicture"></div>
                    <b>Michał Buszka</b>
                </div>
            </div>
            <div className="userSettings">
                <button>Settings</button>
                <button onClick={() => {logout()}}>Logout</button>
            </div>
        </div>
    );
}

export default ConversationsList;