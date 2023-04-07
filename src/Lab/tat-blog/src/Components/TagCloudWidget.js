import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getTagCloud } from "../Services/Widget";

const TagCloudWidget = () => {
    const [tagCloud, setTagCloud] = useState([]);

    useEffect(() => {
        getTagCloud().then((data) => {
            if (data) {
                setTagCloud(data.items);
            } else {
                setTagCloud([]);
            }
        });
    }, []);

    return (
        <div className="mb-4">
            <h4 className="text-success mb-2">Tag Cloud</h4>
            {tagCloud.length > 0 && (
                <ListGroup>
                    {tagCloud.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link
                                    to={`/blog/tag?slug=${item.urlSlug}`}
                                    title={item.name}
                                    key={index}
                                >
                                    {item.name}
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            )}
        </div>
    );
};

export default TagCloudWidget;
