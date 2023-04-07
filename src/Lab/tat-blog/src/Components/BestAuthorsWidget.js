import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getBestAuthors } from "../Services/Widget";

const BestAuthorsWidget = () => {
    const [bestAuthors, setBestAuthors] = useState([]);

    useEffect(() => {
        getBestAuthors().then((data) => {
            if (data) {
                setBestAuthors(data);
            } else {
                setBestAuthors([]);
            }
        });
    }, []);

    return (
        <div className="mb-4">
            <h4 className="text-success mb-2">Top 4 tác giả có nhiều bài viết</h4>
            {bestAuthors.length > 0 && (
                <ListGroup>
                    {bestAuthors.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link
                                    to={`/blog/author?slug=${item.urlSlug}`}
                                    title={item.fullName}
                                    key={index}
                                >
                                    {item.fullName}
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            )}
        </div>
    );
};

export default BestAuthorsWidget;
