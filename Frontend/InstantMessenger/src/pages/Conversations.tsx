import ConversationsList from "../components/ConversationList"
import Conversation from "../components/Conversation"
import FriendDetails from "../components/FriendDetails"
import '../Styles/Global.css'
import '../Styles/Conversations.css'
import { useEffect, useState } from "react"
import api, {checkAuthWithBackend} from '../api/api.tsx';
import UserSettingsModal from "../modals/UserSettingsModal"
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { toast } from 'react-toastify';

function Conversations() {
    const [isUserSettingsModalOpen, setIsUserSettingsModalOpen] = useState(false);

    const handleSaveSettings = async (data: any) => {
        setIsUserSettingsModalOpen(false);
        var response = await api.post('/api/User/updateUserData', data);
        if (response.status == 200)
            toast.success("The data has been saved!");
    };

    const logout = async () => {
        var response = await api.get('/api/LoginRegister/logout');
        if (response.status == 200) {
            location.href = '/login';
        }

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
                    <button onClick={() => setIsUserSettingsModalOpen(true)}>User settings</button>
                    <button onClick={logout}>Logout</button>
                    <ToastContainer position="top-right" autoClose={3000} />
                </div>
            </div>
            <Conversation />
            <FriendDetails />
        </div>
    )
}

export default Conversations;