import './App.css'
import { Routes, Route, BrowserRouter } from 'react-router-dom'
import Login from './pages/Login.tsx';
import Register from './pages/Register.tsx';
import Conversations from './pages/Conversations.tsx';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/:id?" element={<Conversations />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App
