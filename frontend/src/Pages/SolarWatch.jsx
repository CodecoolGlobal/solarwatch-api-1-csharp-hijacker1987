import { useState } from "react";
import { useNavigate } from "react-router-dom";
import SolarWatch from "../Components/MainFunctionality/MainFunctionality";
import Cookies from "js-cookie";
import Loading from "../Components/Loading/Loading";

const GetCountry = async (city, token) => {
  try {
    console.log(city);
    const response = await fetch(`http://localhost:5127/SolarWatch/Get?name=${city}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const errorMessage = `HTTP error! Status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error occurred during fetch:", error);
    throw error;
  }
};

const PostCountry = async (city, token) => {
  try {
    const response = await fetch(`http://localhost:5127/CrudAdmin/Post?name=${city}`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const errorMessage = `HTTP error! Status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error occurred during fetch:", error);
    throw error;
  }
};

const DeleteCountry = async (id, token) => {
  try {
    const response = await fetch(`http://localhost:5127/CrudAdmin/Delete?id=${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      const errorMessage = `HTTP error! Status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const data = await response;
    return data;
  } catch (error) {
    console.error("Error occurred during fetch:", error);
    throw error;
  }
};

const CountryGet = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const handleGet = (city) => {
    setLoading(true);
  
    const token = getToken();
    console.log(token);
    GetCountry(city, token)
      .then((data) => {
        setLoading(false);
        navigate(`/city/${city}`, {state: {data} });
      })
      .catch((error) => {
        setLoading(false);
        alert(`There is no City: ${city} in our database!`);
        console.error("Error occurred during login:", error);
      });
  };

  const handlePost = (city) => {
    setLoading(true);
  
    const token = getToken();
  
    PostCountry(city, token)
      .then((data) => {
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
        console.error(`Error addign ${city} to the database!`, error);
      });
  };

  const handleDelete = (id) => {
    setLoading(true);
    const token = getToken();
  
    DeleteCountry(id, token)
      .then((data) => {
        setLoading(false);
        alert(`City with id (${id}) deleted from the database!`);
      })
      .catch((error) => {
        setLoading(false);
        console.error(`Error deleting id: (${id}) from the database!`, error);
      });
  }

  const getToken = () => {
    return Cookies.get("jwtToken");
  }

  const handleCancel = () => {
    navigate("/");
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <SolarWatch
      onGet = { handleGet }
      onPost = { handlePost }
      onDelete = { handleDelete }
      onCancel = { handleCancel }
    />
  );
};

export default CountryGet;



