import pandas as pd
from sklearn.pipeline import Pipeline
from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import OneHotEncoder
from sklearn.linear_model import LinearRegression
from xgboost import XGBRegressor
import joblib

df = pd.read_csv("tips.csv")

X = df[["total_bill","size","smoker","day","time"]]
y = df["tip"]

categorical_features = ["smoker","day","time"]
numeric_features = ["total_bill", "size"]


preprocessor = ColumnTransformer(
  transformers=[
    ("cat", OneHotEncoder(handle_unknown="ignore"),categorical_features),
    ("num","passthrough", numeric_features)
  ]
)

model = Pipeline(
  steps=[
    ("preprocessor",preprocessor),
    ("regressor", XGBRegressor(
      n_estimators = 300,
      max_depth = 5,
      learning_rate = 0.05,
      random_state = 42
    )),
  ]
)

model.fit(X,y)

joblib.dump(model, "tip_pipeline.pkl")

print("Model trained and saved as tip_pipeline.pkl")