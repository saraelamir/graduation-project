import "./App.css";
import { Route, Routes, useLocation } from "react-router-dom";
import Home from "./Pages/Home";
import Feedback from "./Pages/Feedback";
import Settings from "./Pages/Settings";
import Welcome from "./Pages/Welcome";
import Help from "./Pages/Help";
import History from "./Pages/History";
import Input from "./Pages/Input";
import Login from "./Pages/Login";
import SignUp from "./Pages/SignUp";
import Landing from "./Pages/Landing";
import NotFound from "./Pages/NotFound";
import Sidebar from "./components/Sidebar";
import SavingGoal from "./Pages/SavingGoal";
import HeaderBar from "./components/HeaderBar";
import VerifyCode from "./Pages/VerifyCode";
import ForgotPassword from './Pages/ForgotPassword';

function App() {
  const location = useLocation();

  const noSidebarRoutes = ["/", "/welcome","/input","/signup","/login","/landing"];

  const currentPath = location.pathname.toLowerCase();

  const showSidebar = !noSidebarRoutes.includes(currentPath);

  return (
   
    <div className="app-container">
      {showSidebar && ( <div className="sidebar-container"> <Sidebar /></div>
      )}

      <div className={`content ${showSidebar ? "with-sidebar" : "full-width"}`}>
        <Routes>
          <Route path="/" element={<Landing />} />
          <Route path="/landing" element={<Landing />} />

          <Route path="/home" element={<Home />} />
          <Route path="/feedback" element={<Feedback />} />
          <Route path="/help" element={<Help />} />
          <Route path="/history" element={<History />} />
          <Route path="/input" element={<Input />} />
          <Route path="/login" element={<Login />} />
          <Route path="/signup" element={<SignUp />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/welcome" element={<Welcome />} />
          <Route path="/saving-goal" element={<SavingGoal />} />
          <Route path="*" element={<NotFound />} /> {/* Catch-all route */}
          <Route path="/verify-code" element={<VerifyCode />} />
          <Route path="/forgotpassword" element={<ForgotPassword />} />

        </Routes>
      </div>
    </div>
  );
}

export default App;