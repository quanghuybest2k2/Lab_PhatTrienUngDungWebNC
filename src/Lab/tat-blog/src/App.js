import "./App.css";
import Footer from "./Components/Footer";
import Layout from "./Pages/Layout";
import Index from "./Pages/Index";
import About from "./Pages/About";
import Contact from "./Pages/Contact";
import Rss from "./Pages/Rss";
import PostDetail from "./Components/Post/PostDetail";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AdminLayout from "./Pages/Admin/Layout";
import * as AdminIndex from "./Pages/Admin/Index";
import Authors from "./Pages/Admin/Author/Authors";
import Categories from "./Pages/Admin/Category/Categories";
import * as CategoryEdit from './Pages/Admin/Category/Edit';
import Comments from "./Pages/Admin/Comment/Comments";
import Posts from "./Pages/Admin/Post/Posts";
import * as PostEdit from "./Pages/Admin/Post/Edit";
import Tags from "./Pages/Admin/Tag/Tags";
import NotFound from "./Pages/NotFound";
import BadRequest from "./Pages/BadRequest";


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path="/" element={<Index />} />
          <Route path="blog" element={<Index />} />
          <Route path="blog/post" element={<PostDetail />} />
          {/* blog/post/2022/9/4/huy-can */}
          <Route
            path="blog/post/:year/:month/:day/:slug"
            element={<PostDetail />}
          />
          <Route path="blog/author" element={<Index />} />
          <Route path="blog/author/:slug" element={<Index />} />
          <Route path="blog/category" element={<Index />} />
          <Route path="blog/category/:slug" element={<Index />} />
          <Route path="blog/tag" element={<Index />} />
          <Route path="blog/archives" element={<Index />} />
          <Route path="blog/Contact" element={<Contact />} />
          <Route path="blog/About" element={<About />} />
          <Route path="blog/Rss" element={<Rss />} />
        </Route>
        <Route path="/admin" element={<AdminLayout />}>
          <Route path="/admin" element={<AdminIndex.default />} />
          <Route path="/admin/authors" element={<Authors />} />
          <Route path="/admin/categories" element={<Categories />} />
          <Route path="/admin/comments" element={<Comments />} />
          <Route path="/admin/posts" element={<Posts />} />
          <Route path="/admin/posts/edit" element={<PostEdit.default />} />
          <Route path="/admin/posts/edit/:id" element={<PostEdit.default />} />
          <Route path="/admin/categories/edit" element={<CategoryEdit.default />} />
          <Route path="/admin/categories/edit/:id" element={<CategoryEdit.default />} />
          <Route path="/admin/tags" element={<Tags />} />
          <Route path="/admin/comments" element={<Comments />} />
        </Route>
        <Route path="/400" element={<BadRequest />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
      <Footer />
    </Router>
  );
}
export default App;
