import React, { useEffect, useState } from "react";
import Button from 'react-bootstrap/Button';
import Table from "react-bootstrap/Table";
import { Link, useParams } from "react-router-dom";
import { getAuthors } from "../../../Services/authorRepository";
import Loading from "../../../Components/Loading";
import AuthorFilterPane from "../../../Components/Admin/AuthorFilterPane";
import { useQuery } from '../../../Utils/Utils';

const Authors = () => {
  const [authorList, setAuthorList] = useState([]);
  const [authorQuery, setAuthorQuery] = useState({});
  const [isVisibleLoading, setIsVisibleLoading] = useState(true);

  let { id } = useParams();
  const query = useQuery();
  const p = query.get('p') ?? 1;
  const ps = query.get('ps') ?? 2;

  useEffect(() => {
    document.title = 'Danh sách tác giả';
    getAuthors().then(
      (data) => {
        if (data) {
          setAuthorQuery((pre) => {
            return { ...pre, to: '/admin/authors' };
          });
          setAuthorList(data.items);
        } else setAuthorList([]);
        setIsVisibleLoading(false);
      }
    );
  }, []);

  return (
    <>
      <h1>Danh sách tác giả</h1>
      <AuthorFilterPane />
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tên tác giả</th>
              <th>Bài viết liên quan</th>
              <th>Email</th>
            </tr>
          </thead>
          <tbody>
            {authorList && authorList.length > 0 ? (
              authorList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link to={`/admin/authors/edit/${item.id}`} className="text-bold">
                      {item.fullName}
                    </Link>
                    <p className="text-muted">
                      Ngày tham gia:{' '}
                      {new Date(item.joinedDate).toISOString().replace('T', ' ').substring(0, 19)}
                    </p>
                  </td>
                  <td>{item.postCount}</td>
                  <td>{item.email}</td>
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

export default Authors;
