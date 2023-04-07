import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getRandomPosts } from "../Services/Widget";

const RandomPostsWidget = () => {
    const [randomPosts, setRandomPosts] = useState([]);

    useEffect(() => {
        getRandomPosts(5).then((data) => {
            if (data) {
                setRandomPosts(data);
            } else {
                setRandomPosts([]);
            }
        });
    }, []);

    return (
        <div className="mb-4">
            <h4 className="text-success mb-2">Top 5 bài viết ngẫu nhiên</h4>
            {randomPosts.length > 0 && (
                <ListGroup>
                    {randomPosts.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link
                                    to={`/blog/post/${item.urlSlug}`}
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

export default RandomPostsWidget;
