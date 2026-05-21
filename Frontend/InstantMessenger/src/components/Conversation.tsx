import '../Styles/Global.css'
import '../Styles/Conversation.css'

function Conversation () {
    return(           
        <div className="conversation">
            <div className="messages"></div>
            <div className="messageType">
                <input type="text" placeholder="Type a message..." />
                <button>Send</button>
            </div>
        </div>
    )
}

export default Conversation;