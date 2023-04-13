import { Link } from 'react-router-dom';
import { useQuery } from '../Utils/Utils';

const BadRequest = () => {
    let query = useQuery(),
        redirectTo = query.get('redirectTo') ?? '/';
    return (
        <>
            <div className='text-center'>
                <h1>400</h1>
                <p className='text-danger'>Có vẻ tham số trong URL của bạn chưa đúng yêu cầu.</p>
                <Link to={"/"}>
                    <button className='btn btn-primary'>
                        Về trang chủ thôi!
                    </button>
                </Link>
            </div>
        </>
    );
}

export default BadRequest