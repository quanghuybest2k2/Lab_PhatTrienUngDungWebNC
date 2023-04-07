import React from 'react';
import { Row, Col, Card } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faThumbsUp } from '@fortawesome/free-solid-svg-icons';

function Comment({ comment }) {
    return (
        <Row className="d-flex mt-3">
            <Col md={8} lg={6}>
                <Card className="shadow-0 border" style={{ backgroundColor: '#f0f2f5' }}>
                    <Card.Body className="p-4">
                        <Card>
                            <Card.Body>
                                <p>{comment.content}</p>
                                <div className="d-flex justify-content-between">
                                    <div className="d-flex flex-row align-items-center">
                                        <p className="h4 mb-0 ms-2">{comment.userName}</p>
                                    </div>
                                    <div className="d-flex flex-row align-items-center">
                                        <p className="h5 text-muted mb-0">{new Date(comment.postDate).toISOString().replace("T", " ").substring(0, 19)}</p>
                                        <FontAwesomeIcon
                                            icon={faThumbsUp}
                                            className="mx-2 fa-xs text-black"
                                            style={{ marginTop: -0.16 + 'rem' }}
                                        />
                                        <p className="h5 text-muted mb-0">3</p>
                                    </div>
                                </div>
                            </Card.Body>
                        </Card>
                    </Card.Body>
                </Card>
            </Col>
        </Row>
    );
}

export default Comment;
