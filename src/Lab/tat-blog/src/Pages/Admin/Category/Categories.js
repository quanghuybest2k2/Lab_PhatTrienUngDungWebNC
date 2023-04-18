import React, { useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { Link, useParams } from 'react-router-dom';
import Loading from '../../../Components/Loading';
import { getCategories } from '../../../Services/categoryRepository';
import CategoryFilterPane from "../../../Components/Admin/CategoryFilterPane";


const Categories = () => {
    const [categoryList, setCategoryList] = useState([]);
    const [categoryQuery, setCategoryQuery] = useState({});
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách chủ đề';
        getCategories().then((data) => {
            if (data) {
                setCategoryQuery((pre) => {
                    return { ...pre, to: '/admin/categories' };
                });
                setCategoryList(data);
            } else setCategoryList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách chủ đề</h1>
            <CategoryFilterPane />
            {isVisibleLoading ? (
                <Loading />
            ) : (
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tiêu đề</th>
                            <th>Bài viết liên quan</th>
                            <th>Hiển thị</th>
                        </tr>
                    </thead>
                    <tbody>
                        {categoryList && categoryList.length > 0 ? (
                            categoryList.map((item, index) => (
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/categories/edit/${item.id}`} className="text-bold">
                                            {item.name}
                                        </Link>
                                        <p className="text-muted">{item.description}</p>
                                    </td>
                                    <td>{item.postCount}</td>
                                    <td>
                                        {item.showOnMenu ? (
                                            <Button variant="success">
                                                Có
                                            </Button>
                                        ) : (
                                            <Button variant="warning">
                                                Không
                                            </Button>
                                        )}
                                    </td>
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
                                    <h4 className="text-danger text-center">Không tìm thấy chủ đề nào</h4>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </Table>
            )}
        </>
    );
};

export default Categories;
