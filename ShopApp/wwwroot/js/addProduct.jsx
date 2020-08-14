
class UploadForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            file: {},
            description: "",
            price: 0,
            count: 0,
            name: "",
        };

        this.submit = this.submit.bind(this);
    }

    submit(e) {
        e.preventDefault();
        const formData = new FormData();
        formData.append('file', this.state.file);
        let product = { Name: this.state.name, Description: this.state.description, Price: this.state.price, Count: this.state.count };
        formData.append('product', JSON.stringify(product));
        $.ajax({
            url: window.location.origin + "/api/newProduct",
            contentType: false,
            cache: false,
            processData: false,
            type: 'POST',
            dataType: 'json',
            data: formData,
        });
    }

    render() {
        return (
            <form onSubmit={e => this.submit(e)}>
                <p>Название товара</p>
                <input type="text" value={this.state.name} onChange={e => this.setState({ name: e.target.value })} />
                <p>Описание</p>
                <textarea type="text" value={this.state.description} onChange={e => this.setState({ description: e.target.value })} />
                <p>Цена</p>
                <input type="number" value={this.state.price} onChange={e => this.setState({ price: e.target.valueAsNumber })} />
                <p>Количество</p>
                <input type="number" value={this.state.count} onChange={e => this.setState({ count: e.target.valueAsNumber })} />
                <p>Загрузить фото</p>
                <input type="file" onChange={e => this.setState({ file: e.target.files[0] })} />
                <button type="submit">Опубликовать</button>
            </form>
        );
    }
}

ReactDOM.render(<UploadForm />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));
