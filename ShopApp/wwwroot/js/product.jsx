let host = "https://localhost:5001/";

let urlOfDomen = "api";

let lastRequest = "";

function LoadData(pattern) {
    let localPattern = host + urlOfDomen + "/" + pattern;
    let data = {};
    if (localPattern === lastRequest)
        return data;

    //$.get(localPattern, responseData => data = responseData);

    let xhr = new XMLHttpRequest();
    xhr.open("get", localPattern, false);
    xhr.onload = () => data = JSON.parse(xhr.responseText);
    xhr.send();

    lastRequest = localPattern;
    return data;
}



class InfoAboutProduct extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: LoadData("product/" + document.location.pathname.substr(document.location.pathname.lastIndexOf("/") + 1)),
        };
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
                <p>
                    1) В VS 2019 файл Package.appxmanifest
                    2) Вкладка Packaging
                    3) Кнопка Choose Certificate
                    4) Кнопка Configure Certificate
                    5) Кнопка Create
                    6) Любой пароль
                    7) В корневую папку приложения
                    8) Файл *_TemporaryKey.pfx
                    9) Публикация приложения в VS с указанием сертификата
                </p>
            </div>
            <div className="product__info__buyProduct">
                <button>
                    Купить
                                </button>
            </div>
            <div className="product__info__showComments">
                <button>
                    Показать комментарии
                            </button>
            </div>
        </div>);
    }
}

ReactDOM.render(<InfoAboutProduct />, document.getElementById("content"));