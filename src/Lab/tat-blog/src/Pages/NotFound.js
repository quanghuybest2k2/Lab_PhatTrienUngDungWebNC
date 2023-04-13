import React from 'react';
import { Link } from 'react-router-dom';

const NotFound = () => {
    return (
        <>
            <div className='text-center'>
                <h1>404 Not Found</h1>
                <p className='text-danger'>Không tìm thấy tài nguyên của trang web</p>
                <Link to={"/"}>
                    <button className='btn btn-primary'>
                        Về trang chủ thôi!
                    </button>
                </Link>
            </div>
        </>
    )
}

export default NotFound