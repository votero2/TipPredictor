from fastapi import FastAPI
from pydantic import BaseModel
import pandas as pd
import joblib

app = FastAPI()

model = joblib.load("tip_pipeline.pkl")



class TipRequest(BaseModel):
  totalBill: float
  size: int
  smoker: str
  day: str
  time: str

@app.get("/")

def home():
  return  {"message": "ML API is running"}

@app.post("/predict")
def predict(request:TipRequest):
  data = pd.DataFrame([{
    "total_bill": request.totalBill,
    "size": request.size,
    "smoker": request.smoker,
    "day": request.day,
    "time": request.time
  }])

  print("DataFrame sent to model:\n",data)

  tip_amount  = model.predict(data)[0]

  raw_tip_amount = float(tip_amount)

  #business rule limits using US tipping behaviors
  predicted_tip_amount = max(request.totalBill * 0.10, tip_amount)
  predicted_tip_amount = min(request.totalBill * 0.35, predicted_tip_amount)


  predicted_tip_pct = predicted_tip_amount / request.totalBill
 

  return {
    "predictedTipPct": round(float(predicted_tip_pct * 100), 2),
    "rawPredictedTipAmount": round(float(raw_tip_amount), 2),
    "adjustedPredictedTipAmount": round(float(predicted_tip_amount), 2),
    "predictedTipAmount": round(float(predicted_tip_amount), 2),
    "message": "ML prediction generated successfully."
  }

