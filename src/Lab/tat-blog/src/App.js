import "./App.css";
import Navbar from "./Components/Navbar";
import Sidebar from "./Components/Sidebar";
import Footer from "./Components/Footer";
import Layout from "./Pages/Layout";
import Index from "./Pages/Index";
import About from "./Pages/About";
import Contact from "./Pages/Contact";
import Rss from "./Pages/Rss";
import PostDetail from './Components/Post/PostDetail';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <div>
      <Router>
        <Navbar />
        <div className="container-fluid">
          <div className="row">
            <div className="col-9">
              <Routes>
                <Route path="/" element={<Layout />}>
                  <Route path="/" element={<Index />} />
                  <Route path="blog" element={<Index />} />
                  <Route path="blog/post" element={<PostDetail />} />
                  {/* blog/post/2022/9/4/huy-can */}
                  <Route path="blog/post/:year/:month/:day/:slug" element={<PostDetail />} />
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
              </Routes>
            </div>
            <div className="col-3 border-start">
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </Router>
    </div>
  );
}

export default App;
