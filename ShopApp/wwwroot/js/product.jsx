
function ProductInfoComment(props) {
    return (<li><p>{props.authorId}</p><p>{props.body}</p></li>);
}

class ProductInfoComments extends React.Component{
    constructor(props) {
        super(props);
        this.state = {
            loadedComments: [],
        }; 
    }
    render(){
        let comments = LoadData("/comments/" + this.props.prodId + "/" + this.props.pageComment, []);
        this.state.loadedComments = this.state.loadedComments.concat(comments);
        return (<ul>{this.state.loadedComments.map((item, i) => <ProductInfoComment authorId={item.authorId} body={item.body} key={item.id} />)}</ul>);
    }
}

class InfoAboutProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: LoadData(window.location.pathname, {}, false),
            // "product/" + document.location.pathname.substr(document.location.pathname.lastIndexOf("/") + 1)
            pageComment: 0,
        };

        this.onShowComments = this.onShowComments.bind(this);
    }

    onShowComments(e) {
        e.preventDefault();
        ReactDOM.render(<ProductInfoComments prodId={this.state.data.id} pageComment={this.state.pageComment} />, document.getElementById("product__info__comments__wrapper"));
        this.setState({ pageComment: this.state.pageComment + 1 });
    }

    render() {
        return (<div className="product__info">
            <div className="product__info__image">
                <img src={host+this.state.data.linkToImage} />
            </div>
            <div className="product__info__basic">
                <div className="product__info__name">
                    <p>{this.state.data.name}</p>
                </div> 
                <div className="product__info__basic__space__between">
                    <div className="product__info__price">
                        <p>{this.state.data.price}</p>
                    </div>
                    <div className="product__info__author">
                        <a>{this.state.data.publisherId}</a>
                    </div>
                </div>
            </div>
            <div className="product__info__description">
                <p>{this.state.data.description}</p>
            </div>
            <div className="product__info__buyProduct">
                <button>Купить</button>
            </div>
            <div className="product__info__showComments">
                <div id="product__info__comments__wrapper">
                </div>
                <button onClick={this.onShowComments}>
                    Показать комментарии
                </button>
            </div>
            <div className="formForNewComment">
            </div> 
        </div>);
    }
}


ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<InfoAboutProduct />, document.getElementById("content"));
