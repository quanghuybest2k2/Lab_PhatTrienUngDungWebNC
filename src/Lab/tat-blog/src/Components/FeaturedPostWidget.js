import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getFeaturedPosts } from "../Services/Widget";

const FeaturedPostWidget = () => {
    const [featuredPost, setfeaturedPostList] = useState([]);

    useEffect(() => {
        getFeaturedPosts().then((data) => {
            if (data) {
                setfeaturedPostList(data);
            } else {
                setfeaturedPostList([]);
            }
        });
    }, []);

    return (
        <div className="mb-4">
            <h4 className="text-success mb-2">Top 3 bài viết xem nhiều nhất</h4>
            {featuredPost.length > 0 && (
                <ListGroup>
                    {featuredPost.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link
                                    to={`/blog/post?slug=${item.urlSlug}`}
                                    title={item.title}
                                    key={index}
                                >
                                    {item.title}
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            )}
        </div>
    );
};

export default FeaturedPostWidget;
