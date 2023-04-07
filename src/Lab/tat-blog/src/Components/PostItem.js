import TagList from './TagList';
import Card from 'react-bootstrap/Card';
import { Link } from 'react-router-dom';
import { isEmptyOrSpaces } from '../Utils/Utils';

const PostList = ({ postItem }) => {
    let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
        ? process.env.PUBLIC_URL + '/images/image_1.jpg'
        : `${postItem.imageUrl}`;
    let postedDate = new Date(postItem.postedDate);
    return (
        <article className='blog-entry mb-4'>
            <Card>
                <div className='row g-0'>
                    <div className='col-md-4'>
                        <Card.Img variant='top' src={imageUrl} alt={postItem.title} />
                    </div>
                    <div className='col-md-8'>
                        <Card.Body>
                            <Card.Title>{postItem.title}</Card.Title>
                            <Card.Text>
                                <small className='text-muted'>Tác giả:</small>
                                <span className='text-primary m-1'>
                                    {/* https://localhost:44309/api/authors/jason-mouth/posts?PageSize=10&PageNumber=1 */}
                                    <Link
                                        to={`/blog/authors/${postItem.author.urlSlug}`}
                                        title={postItem.author.description}
                                        key={postItem.author}
                                    >
                                        {postItem.author.fullName}
                                    </Link>
                                </span>
                                <small className='text-muted'>Chủ đề:</small>
                                <span className='text-primary m-1'>
                                    <Link
                                        to={`/blog/category/${postItem.category.urlSlug}`}
                                        title={postItem.category.description}
                                        key={postItem.category}
                                    >
                                        {postItem.category.name}
                                        {/* <span>&nbsp;({postItem.category.postCount})</span> */}
                                    </Link>
                                </span>
                            </Card.Text>
                            <Card.Text>
                                {postItem.shortDescription}
                            </Card.Text>
                            <div className='tag-list'>
                                <TagList tagList={postItem.tags} />
                            </div>
                            <div className='text-end'>
                                <Link to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth() + 1}/${postedDate.getDate()}/${postItem.urlSlug}`}
                                    className='btn btn-primary'
                                    title={postItem.title}>
                                    Xem chi tiết
                                </Link>
                            </div>
                        </Card.Body>
                    </div>
                </div>
            </Card>
        </article>
    )
}

export default PostList;