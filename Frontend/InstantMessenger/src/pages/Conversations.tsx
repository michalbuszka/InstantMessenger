import { useEffect } from "react"

function Conversations() {
    useEffect(() => {
        const token = localStorage.getItem('token');
        if (!token) {
            window.location.href = '/login';
        }
    }, []);
    return (
        <h1>Conversations</h1>
    )
}
export default Conversations