import React, { useEffect, useState } from "react";
import { Form, InputGroup } from "react-bootstrap";
import { BsSearch } from "react-icons/bs";
import axios from "axios";

const HeaderBar = () => {
  const [user, setUser] = useState({
    name: "Maged ",
    imageUrl: "https://via.placeholder.com/40",
  });

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await axios.get(
          "https://graduationproj.runasp.net/api/UserProfile/profile",
          {
            headers: {
              Authorization: "Bearer YOUR_TOKEN_HERE", // ← ضيفي التوكن هنا
              Accept: "/",
            },
          }
        );

        if (response.data?.name && response.data?.imageUrl) {
          setUser(response.data);
        }
      } catch (err) {
        console.error("فشل في تحميل بيانات المستخدم", err);
      }
    };

    fetchUser();
  }, []);

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-end align-items-center gap-3 p-3">
        {/* Search */}
        <InputGroup style={{ width: "260px" ,height:"px"}}>
          <InputGroup.Text
            style={{
              backgroundColor: "white",
              borderRight: "none",
              borderRadius: "10px 0 0 10px",
            }}
          >
            <BsSearch />
          </InputGroup.Text>
          <Form.Control
            type="text"
            placeholder="Search here..."
            style={{
              borderLeft: "none",
              borderRadius: "0 10px 10px 0",
            }}
          />
        </InputGroup>

        {/* Name and Image */}
        <div
          className="d-flex align-items-center gap-2 px-2 py-1"
          style={{
            backgroundColor: "#f1f1f1",
            borderRadius: "999px",
            height: "35px", // ← أصغر من الأول
          }}
        >
          <span className="fw-bold" style={{ fontSize: "14px" }}>
            {user.name}
          </span>
          <img
            src={user.imageUrl}
            alt="User"
            className="rounded-circle"
            style={{
              width: "24px", // ← أصغر من 35px
              height: "24px", // ← أصغر من 35px
              objectFit: "cover",
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default HeaderBar;
