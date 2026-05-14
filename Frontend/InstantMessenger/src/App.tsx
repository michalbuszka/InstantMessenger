import './App.css'
import { Routes, Route, BrowserRouter } from 'react-router-dom'
import Home from './pages/Home.tsx'
import Login from './pages/Login.tsx';
import Register from './pages/Register.tsx';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App
