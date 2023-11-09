import { useState } from "react";
import { useNavigate } from "react-router-dom";
import UserForm from "../Components/UserForm";
import Loading from "../Components/Loading/Loading";

const createUser = (user) => {
  console.log(JSON.stringify(user));
    return fetch("http://localhost:8080/Auth/Register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(user),
    }).then((res) => res.json());
  };

const UserCreator = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleCreateUser = (user) => {
    setLoading(true);

    createUser(user)
      .then(() => {
        setLoading(false);
        navigate("/");
      })
    };

    const handleCancel = () => {
        navigate("/");
    };

    if (loading) {
      return <Loading />;
    }

  return (
    <UserForm
        onSave = { handleCreateUser }
        onCancel = { handleCancel }
    />
  );
};

export default UserCreator;
