import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getCurrentUser } from "../services/userService";
import { changePassword } from "../services/authService";
import Skeleton from "../components/skeleton/Skeleton";
import "./styles/Settings.css";


function Settings()
{

    const navigate = useNavigate();


    const [showReset,setShowReset] = useState(false);


    const [user,setUser] = useState(null);


    const [loading,setLoading] = useState(true);



    const [showCurrentPassword,setShowCurrentPassword] = useState(false);

    const [showNewPassword,setShowNewPassword] = useState(false);

    const [showConfirmPassword,setShowConfirmPassword] = useState(false);



    const [currentPassword,setCurrentPassword] = useState("");

    const [newPassword,setNewPassword] = useState("");

    const [confirmPassword,setConfirmPassword] = useState("");





    useEffect(() =>
    {

        async function loadUser()
        {

            try
            {

                const data =
                    await getCurrentUser();


                setUser(data);


            }
            catch(error)
            {

                console.log(error);

            }
            finally
            {

                setLoading(false);

            }

        }


        loadUser();


    },[]);








    async function handleResetPassword()
    {


        if(newPassword !== confirmPassword)
        {

            alert(
                "Passwords do not match"
            );

            return;

        }



        try
        {


            const response =
                await changePassword(
                {

                    currentPassword,

                    newPassword

                });



            alert(
                response.message
            );



            setShowReset(false);



            setCurrentPassword("");

            setNewPassword("");

            setConfirmPassword("");


        }
        catch(error)
        {


            console.log(error);



            alert(
                error.response?.data?.message
                ||
                "Password change failed"
            );


        }


    }









    return (

        <div className="settings-page">



            <div className="settings-card">



                <div className="settings-header">


                    <h1>
                        Settings
                    </h1>




                    <button

                    className="back-btn"

                    onClick={() =>
                        navigate("/dashboard")
                    }

                    >

                        Back

                    </button>


                </div>









                <div className="profile-section">


                    <h2>
                        Profile Information
                    </h2>





                    {

                    loading

                    ?

                            <>

                            <div className="profile-item">

                                <strong>Name</strong>

                                <Skeleton width="180px" />

                            </div>


                            <div className="profile-item">

                                <strong>Email</strong>

                                <Skeleton width="250px" />

                            </div>


                            <div className="profile-item">

                                <strong>Role</strong>

                                <Skeleton width="120px" />

                            </div>


                            <div className="profile-item">

                                <strong>Status</strong>

                                <Skeleton width="100px" />

                            </div>

                            </>


                    :


                    <>


                    <div className="profile-item">

                        <strong>
                            Name:
                        </strong>


                        <span>
                            {user?.name}
                        </span>


                    </div>






                    <div className="profile-item">

                        <strong>
                            Email:
                        </strong>


                        <span>
                            {user?.email}
                        </span>


                    </div>






                    <div className="profile-item">

                        <strong>
                            Role:
                        </strong>


                        <span>
                            {user?.role}
                        </span>


                    </div>






                    <div className="profile-item">

                        <strong>
                            Status:
                        </strong>


                        <span>

                            {
                            user?.isActive
                            ?
                            "Active"
                            :
                            "Inactive"
                            }

                        </span>


                    </div>



                    </>

                    }








                    <button

                    className="reset-btn"

                    onClick={() =>
                        setShowReset(true)
                    }

                    >

                        Reset Password

                    </button>



                </div>



            </div>









            {

            showReset &&



            <div className="modal-overlay">



                <div className="reset-modal">



                    <h2>
                        Reset Password
                    </h2>








                    <label>
                        Current Password
                    </label>



                    <div className="password-container">


                        <input

                        type={
                            showCurrentPassword
                            ?
                            "text"
                            :
                            "password"
                        }

                        value={currentPassword}

                        onChange={(e)=>
                            setCurrentPassword(
                                e.target.value
                            )
                        }

                        />



                        <button

                        type="button"

                        className="show-password-btn"

                        onClick={() =>
                            setShowCurrentPassword(
                                !showCurrentPassword
                            )
                        }

                        >

                            {
                            showCurrentPassword
                            ?
                            "Hide"
                            :
                            "Show"
                            }


                        </button>



                    </div>


                    <label>
                        New Password
                    </label>



                    <div className="password-container">


                        <input

                        type={
                            showNewPassword
                            ?
                            "text"
                            :
                            "password"
                        }

                        value={newPassword}

                        onChange={(e)=>
                            setNewPassword(
                                e.target.value
                            )
                        }

                        />



                        <button

                        type="button"

                        className="show-password-btn"

                        onClick={() =>
                            setShowNewPassword(
                                !showNewPassword
                            )
                        }

                        >

                            {
                            showNewPassword
                            ?
                            "Hide"
                            :
                            "Show"
                            }


                        </button>



                    </div>









                    <label>
                        Confirm New Password
                    </label>



                    <div className="password-container">


                        <input

                        type={
                            showConfirmPassword
                            ?
                            "text"
                            :
                            "password"
                        }

                        value={confirmPassword}

                        onChange={(e)=>
                            setConfirmPassword(
                                e.target.value
                            )
                        }

                        />



                        <button

                        type="button"

                        className="show-password-btn"

                        onClick={() =>
                            setShowConfirmPassword(
                                !showConfirmPassword
                            )
                        }

                        >

                            {
                            showConfirmPassword
                            ?
                            "Hide"
                            :
                            "Show"
                            }


                        </button>



                    </div>









                    <div className="modal-buttons">



                        <button

                        className="cancel-btn"

                        onClick={() =>
                            setShowReset(false)
                        }

                        >

                            Cancel

                        </button>






                        <button

                        className="confirm-btn"

                        onClick={handleResetPassword}

                        >

                            Confirm

                        </button>




                    </div>





                </div>



            </div>


            }






        </div>

    );

}



export default Settings;