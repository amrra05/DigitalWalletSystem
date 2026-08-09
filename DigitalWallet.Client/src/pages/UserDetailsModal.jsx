import "./UserDetailsModal.css";


function UserDetailsModal({
    user,
    onClose,
    onFreeze,
    onResetPassword
})
{


    if(!user)
        return null;



    return (

        <div className="modal-overlay">


            <div className="user-modal">


                <h2>
                    User Details
                </h2>



                <div className="user-info">


                    <p>
                        <b>Name:</b>
                        {" "}
                        {user.name}
                    </p>


                    <p>
                        <b>Email:</b>
                        {" "}
                        {user.email}
                    </p>


                    <p>
                        <b>Role:</b>
                        {" "}
                        {user.role}
                    </p>


                    <p>
                        <b>Balance:</b>
                        {" "}
                        {user.balance} JOD
                    </p>


                    <p>
                        <b>Created:</b>
                        {" "}
                        {new Date(
                            user.createdAt
                        ).toLocaleDateString()}
                    </p>



                    <p>
                        <b>Status:</b>
                        {" "}
                        {
                            user.isActive
                            ?
                            "Active"
                            :
                            "Frozen"
                        }
                    </p>


                </div>



                <div className="modal-actions">


                    <button
                    className="freeze-btn"
                    onClick={onFreeze}
                    >

                    {
                        user.isActive
                        ?
                        "Freeze User"
                        :
                        "Unfreeze User"
                    }


                    </button>



                    <button
                    className="reset-btn"
                    onClick={onResetPassword}
                    >

                        Reset Password

                    </button>



                    <button
                    className="close-btn"
                    onClick={onClose}
                    >

                        Close

                    </button>


                </div>


            </div>


        </div>

    );

}


export default UserDetailsModal;