import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Login from "../Components/Login/Login";
import Cookies from "js-cookie";
import Loading from "../Components/Loading/Loading";

const postLogin = (user) => {
    return fetch("http://localhost:5120/Auth/Login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    }).then((res) => res.json());
  };

const UserLogin = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleOnLogin = (user) => {
    setLoading(true);
  
    postLogin(user)
      .then((data) => {
        setLoading(false);
        if (data.token) {
          Cookies.set("jwtToken", data.token, { expires: 1 });
          navigate("/");
        } else {
          console.log("Login unsuccessful. Please check your credentials.");
        }
      })
      .catch((error) => {
        setLoading(false);
        console.error("Error occurred during login:", error);
      });
  };

  const handleCancel = () => {
      navigate("/");
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <Login
      onLogin = { handleOnLogin }
      onCancel = { handleCancel }
    />
  );
};

export default UserLogin;
