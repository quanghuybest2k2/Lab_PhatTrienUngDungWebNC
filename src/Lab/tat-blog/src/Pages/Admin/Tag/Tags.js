import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { Link } from 'react-router-dom';
import Loading from '../../../Components/Loading';
import { getTags } from '../../../Services/tagRepository';
import TagFilterPane from '../../../Components/Admin/TagFilterPane';

const Tags = () => {
    const [tagList, setTagList] = useState([]);
    const [tagQuery, setTagQuery] = useState({});
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách thẻ';
        getTags().then((data) => {
            if (data) {
                setTagQuery((pre) => {
                    return { ...pre, to: '/admin/tags' };
                });
                setTagList(data.items);
            } else setTagList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách thẻ</h1>
            <TagFilterPane />
            {isVisibleLoading ? (
                <Loading />
            ) : (
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tên thẻ</th>
                            <th>Bài viết liên quan</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tagList && tagList.length > 0 ? (
                            tagList.map((item, index) => (
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/tags/edit/${item.id}`} className="text-bold">
                                            {item.name}
                                        </Link>
                                        <p className="text-muted">{item.description}</p>
                                    </td>
                                    <td>{item.postCount}</td>
                                    <td>
                                        <Button variant="danger">
                                            Xoá
                                        </Button>
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan={4}>
                                    <h4 className="text-danger text-center">Không tìm thấy tác giả nào</h4>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            )}
        </>
    );
};

export default Tags;
