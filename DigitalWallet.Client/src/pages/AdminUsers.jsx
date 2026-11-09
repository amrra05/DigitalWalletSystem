import { useEffect, useState } from "react";
import { getAdminUsers } from "../services/adminService";
import Skeleton from "../components/skeleton/Skeleton";
import BackButton from "../components/BackButton";
import {
    freezeUser,
    unfreezeUser,
    resetUserPassword
}
from "../services/adminService";
import "./styles/AdminUsers.css";


function ManageUsers()
{

    const [users,setUsers] = useState([]);

    const [loading,setLoading] = useState(true);


    const [page,setPage] = useState(1);

    const [totalPages,setTotalPages] = useState(1);


    const [search,setSearch] = useState("");


    const [selectedUser,setSelectedUser] = useState(null);

    const [resetUser,setResetUser] = useState(null);

const [newPassword,setNewPassword] = useState("");

const [resetMessage,setResetMessage] = useState("");




    async function loadUsers()
    {

        try
        {

            setLoading(true);


            const data =
                await getAdminUsers({

                    page,

                    pageSize:10,

                    search

                });



            setUsers(data.items);


            setTotalPages(
                data.totalPages
            );


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





    useEffect(()=>{

        loadUsers();

    },[page,search]);




async function handleFreeze(id)
{

    try
    {

        await freezeUser(id);


        setSelectedUser(null);


        loadUsers();


    }
    catch(error)
    {

        console.log(error);

    }

}



async function handleUnfreeze(id)
{

    try
    {

        await unfreezeUser(id);


        setSelectedUser(null);


        loadUsers();


    }
    catch(error)
    {

        console.log(error);

    }

}

async function handleResetPassword()
{

    try
    {

        await resetUserPassword(
            resetUser.id,
            {
                newPassword
            }
        );


        setResetMessage(
            "Password reset successfully"
        );


        setNewPassword("");

    }
    catch(error)
    {

        console.log(error);


        setResetMessage(
            "Password reset failed"
        );

    }

}


    return (

        <div className="admin-users-page">


            <h1>
                Manage Users
            </h1>



            <BackButton />





            <div className="users-toolbar">


                <input

                    className="users-search"

                    placeholder="Search users..."

                    value={search}


                    onChange={(e)=>{

                        setPage(1);

                        setSearch(
                            e.target.value
                        );

                    }}


                />


            </div>









            {
            loading ?


            <>


                <Skeleton height="60px"/>

                <Skeleton height="60px"/>

                <Skeleton height="60px"/>


            </>



            :



            <table className="users-table">


                <thead>


                    <tr>


                        <th>
                            ID
                        </th>


                        <th>
                            Name
                        </th>


                        <th>
                            Email
                        </th>


                        <th>
                            Role
                        </th>


                        <th>
                            Status
                        </th>


                        <th>
                            Action
                        </th>


                    </tr>


                </thead>





                <tbody>


                {
                    users.map(user=>(


                        <tr key={user.id}>


                            <td>
                                {user.id}
                            </td>



                            <td>
                                {user.name}
                            </td>




                            <td>
                                {user.email}
                            </td>




                            <td>
                                {user.role}
                            </td>





                            <td>


                                <span

                                className={
                                    user.isActive
                                    ?
                                    "status-active"
                                    :
                                    "status-frozen"
                                }


                                >

                                {
                                    user.isActive
                                    ?
                                    "Active"
                                    :
                                    "Frozen"
                                }


                                </span>


                            </td>






                            <td>


                                <button

                                className="view-btn"


                                onClick={()=>
                                    setSelectedUser(user)
                                }


                                >

                                    View

                                </button>


                            </td>




                        </tr>


                    ))
                }



                </tbody>



            </table>



            }









            <div className="pagination">



                <button


                disabled={
                    page === 1
                }


                onClick={()=>
                    setPage(page-1)
                }


                >

                    Previous

                </button>





                <span>

                    Page {page} of {totalPages}

                </span>





                <button


                disabled={
                    page === totalPages
                }


                onClick={()=>
                    setPage(page+1)
                }


                >

                    Next

                </button>




            </div>









            {
            selectedUser &&


            <div className="modal-overlay">



                <div className="user-modal">



                    <h2>
                        User Details
                    </h2>




                    <p>

                        <strong>
                            ID:
                        </strong>

                        {" "}

                        {selectedUser.id}

                    </p>




                    <p>

                        <strong>
                            Name:
                        </strong>

                        {" "}

                        {selectedUser.name}

                    </p>





                    <p>

                        <strong>
                            Email:
                        </strong>

                        {" "}

                        {selectedUser.email}

                    </p>





                    <p>

                        <strong>
                            Role:
                        </strong>

                        {" "}

                        {selectedUser.role}

                    </p>





                    <p>

                        <strong>
                            Wallet Balance:
                        </strong>

                        {" "}

                        {selectedUser.balance}

                        {" JOD"}

                    </p>





                    <p>

                        <strong>
                            Created:
                        </strong>

                        {" "}


                        {
                        new Date(
                            selectedUser.createdAt
                        )
                        .toLocaleDateString()
                        }


                    </p>








                    <div className="modal-actions">





                        <button

                        className="freeze-btn"
                        onClick={() =>
                            {
                                if(selectedUser.isActive)
                                {
                                    handleFreeze(selectedUser.id);
                                }
                                else
                                {
                                    handleUnfreeze(selectedUser.id);
                                }
                            }}
                        >

                        {
                            selectedUser.isActive
                            ?
                            "Freeze User"
                            :
                            "Unfreeze User"
                        }


                        </button>







                    <button

                    className="reset-btn"

                    onClick={() =>
                    {
                        setResetUser(selectedUser);
                        setResetMessage("");
                    }}

                    >

                    Reset Password

                    </button>







                        <button

                        className="close-btn"


                        onClick={()=>
                            setSelectedUser(null)
                        }


                        >

                            Close


                        </button>


{
resetUser &&

<div className="modal-overlay">


    <div className="user-modal reset-modal">


        <h2>
            Reset Password
        </h2>



        <p>
            User:
            {" "}
            <strong>
                {resetUser.email}
            </strong>
        </p>



        <input

        type="password"

        placeholder="Enter new password"

        value={newPassword}

        onChange={(e)=>
            setNewPassword(e.target.value)
        }

        />



        {
            resetMessage &&

            <p className="reset-message">

                {resetMessage}

            </p>
        }



        <div className="modal-actions">


            <button

            className="reset-btn"

            onClick={handleResetPassword}

            >

                Confirm Reset

            </button>




            <button

            className="close-btn"

            onClick={() =>
            {
                setResetUser(null);
                setNewPassword("");
            }}

            >

                Close

            </button>



        </div>



    </div>


</div>
}


                    </div>





                </div>





            </div>


            }



        </div>

    );

}



export default ManageUsers;