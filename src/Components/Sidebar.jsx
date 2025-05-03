import { Link } from "react-router-dom";
import { FaHome, FaComment, FaQuestionCircle, FaHistory, FaCog, FaPiggyBank, FaSignOutAlt } from "react-icons/fa";
import "bootstrap/dist/css/bootstrap.min.css";
import styles from './Sidebar.module.css';  // استيراد الموديول

import logo from "../assets/Logo Icon.png";

const Sidebar = () => {
  return (
    <div className={styles.sidebar}>
      <h4 className={`${styles.logo} fw-bold mb-4`}>
        <img src={logo} alt="" className={styles.img} />Wealth Wise
      </h4>
      <ul className="list-unstyled">
        <li className={styles.mb3}>
          <Link to="/home" className={`${styles.link} text-dark`}>
            <FaHome className={styles.icon} /> Home
          </Link>
        </li>
        <li className={styles.mb3}>
          <Link to="/feedback" className={`${styles.link} text-dark`}>
            <FaComment className={styles.icon} /> Feedback
          </Link>
        </li>
        <li className={styles.mb3}>
          <Link to="/help" className={`${styles.link} text-dark`}>
            <FaQuestionCircle className={styles.icon} /> Help
          </Link>
        </li>
        <li className={styles.mb3}>
          <Link to="/history" className={`${styles.link} text-dark`}>
            <FaHistory className={styles.icon} /> History
          </Link>
        </li>
        <li className={styles.mb3}>
          <Link to="/settings" className={`${styles.link} text-dark`}>
            <FaCog className={styles.icon} /> Settings
          </Link>
        </li>
        <li className={styles.mb3}>
          <Link to="/saving-goal" className={`${styles.link} text-dark`}>
            <FaPiggyBank className={styles.icon} /> SavingGoal
          </Link>
        </li>
        <li className={styles.mt4}>
          <Link to="/logout" className={`${styles.logoutLink} d-flex align-items-center`}>
            <FaSignOutAlt className={styles.icon} /> Logout
          </Link>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
