import { useState } from "react";
import { Link } from "react-router-dom";
import {
  FaHome,
  FaComment,
  FaQuestionCircle,
  FaHistory,
  FaCog,
  FaPiggyBank,
  FaSignOutAlt,
  FaBars,
  FaChevronLeft,
} from "react-icons/fa";
import "bootstrap/dist/css/bootstrap.min.css";
import styles from "./Sidebar.module.css";
import logo from "../assets/Logo Icon.png";

const Sidebar = () => {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <div className={collapsed ? styles.sidebarCollapsed : styles.sidebar}>
      <button
        className={styles.toggleBtn}
        onClick={() => setCollapsed(!collapsed)}
        aria-label={collapsed ? "Expand sidebar" : "Collapse sidebar"}
      >
        {collapsed ? <FaBars /> : <FaChevronLeft />}
      </button>

      <h4 className={`${styles.logo} fw-bold mb-4`}>
        {!collapsed && (
          <>
            <img src={logo} alt="Wealth Wise logo" className={styles.img} />
            Wealth Wise
          </>
        )}
      </h4>

      <ul className="list-unstyled">
        {[
          { to: "/home", icon: FaHome, label: "Home" },
          { to: "/feedback", icon: FaComment, label: "Feedback" },
          { to: "/help", icon: FaQuestionCircle, label: "Help" },
          { to: "/history", icon: FaHistory, label: "History" },
          { to: "/settings", icon: FaCog, label: "Settings" },
          { to: "/input", icon: FaPiggyBank, label: "Input" },
        ].map(({ to, icon: Icon, label }, idx) => (
          <li key={idx} className={styles.mb3}>
            <Link to={to} className={`${styles.link} text-dark`}>
              {Icon && <Icon className={styles.icon} />}
              {!collapsed && label}
            </Link>
          </li>
        ))}

        <li className={styles.mt4}>
          <Link
            to="/landing"
            className={`${styles.logoutLink} d-flex align-items-center`}
          >
            <FaSignOutAlt className={styles.icon} />
            {!collapsed && "Logout"}
          </Link>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
