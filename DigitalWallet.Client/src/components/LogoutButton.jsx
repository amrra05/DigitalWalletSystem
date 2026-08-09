import { useContext } from "react";
import { useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import "./LogoutButton.css";


function LogoutButton()
{

    const { logout } = useContext(AuthContext);

    const navigate = useNavigate();



    function handleLogout()
    {

        logout();

        navigate("/login");

    }



    return (

        <button

            className="logout-button"

            onClick={handleLogout}

        >

            Logout

        </button>

    );

}


export default LogoutButton;