import LoginRegister from './components/LoginRegister'

import './app.css'
import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './ProtectedRoute'
import Dashboard from './Dashboard'

function App() {





  return (
    <>
    <BrowserRouter>
    <Routes>
      <Route path="/" element={<Navigate to="/login"/>}/>
      <Route path="/login" element={<LoginRegister/>}/>
      <Route path="/dashboard"
      element={
        <ProtectedRoute>
          <Dashboard/>
        </ProtectedRoute>
      }/>
      <Route
      path="/categories"
      element={
        <ProtectedRoute>
          <Dashboard/>
        </ProtectedRoute>
      }/>
    </Routes>
    </BrowserRouter>
   
    </>
  )
}

export default App
