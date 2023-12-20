import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import Loading from "../Components/Loading/Loading";
import PassChange from "../Components/PassChange/PassChange";

const patchPwChange = async (user, token) => {
    try {
        console.log(user);
      const response = fetch("http://localhost:5127/Auth/ChangePassword", {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(user)
      })
      const data = await response;
      return data;
    } catch (error) {
      console.error("Error occurred during fetch:", error);
      throw error;
    }
  }

const PwChange = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleUserPasswordUpdate = (user) => {
    setLoading(true);

    const token = getToken();
    
    patchPwChange(user, token)
      .then(() => {
        setLoading(false);
        navigate("/");
      })
      .catch((error) => {
        setLoading(false);
        console.error("Error occurred during password change:", error);
      });
    };


  const handleCancel = () => {
      navigate("/");
  };

  const getToken = () => {
    return Cookies.get("jwtToken");
  }

  if (loading) {
    return <Loading />;
  }

  return (
    <PassChange
      onPassChange = { handleUserPasswordUpdate }
      onCancel = { handleCancel }
    />
  );
};

export default PwChange;
